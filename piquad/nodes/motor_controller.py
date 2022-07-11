import os
import time

import pigpio
import logging

from exceptions.exceptions import InvalidArgumentException
from piquad.motors.motor import Motor
from piquad.motors.motor_location import MotorLocation

THROTTLE_MIN = os.environ.get("THROTTLE_MIN") or 1000
THROTTLE_MAX = os.environ.get("THROTTLE_MAX") or 2000


class MotorController:

    def __init__(self, bus):
        super().__init__()

        self.pi = pigpio.pi()
        self.logger = logging.getLogger('piquad')

        self.throttle = 0
        self.yaw = 0
        self.pitch = 0
        self.roll = 0

        self.motors = {MotorLocation.BACK_RIGHT: Motor(self.pi, os.environ.get("GPIO_PIN_M1") or 23),
                       MotorLocation.FRONT_RIGHT: Motor(self.pi, os.environ.get("GPIO_PIN_M2") or 24),
                       MotorLocation.BACK_LEFT: Motor(self.pi, os.environ.get("GPIO_PIN_M3") or 25),
                       MotorLocation.FRONT_LEFT: Motor(self.pi, os.environ.get("GPIO_PIN_M4") or 16)}

        bus.create_subscriber(str, 'motor-controller', self.handle_msg, 10)

    def handle_msg(self, msg):
        operation, value = msg.split(':')

        if operation not in ['throttle', 'yaw', 'pitch', 'roll']:
            raise InvalidArgumentException(f'Invalid operation: {operation} given!')

        setattr(self, operation, value)
        self.update_motor_speed()

    def update_motor_speed(self, location=MotorLocation.ALL):
        if location == MotorLocation.ALL:
            for location in self.motors.keys():
                self.update_motor_speed(location)
            return self.logger.info(self.info())

        # self.motors[location].speed = self.calc_motor_speed(location)
        self.motors[location].speed = (self.throttle * 1000) + 1000

    # def get_motor_yaw(self, location):
    #     match location:
    #         case MotorLocation.FRONT_RIGHT | MotorLocation.BACK_LEFT:
    #             return 0
    #         case MotorLocation.FRONT_LEFT | MotorLocation.BACK_RIGHT:
    #             return self.yaw
    #
    #     raise InvalidArgumentException(f'Motor location {location} does not exist!')
    #
    # def get_motor_pitch(self, location):
    #     match location:
    #         case MotorLocation.BACK_LEFT | MotorLocation.BACK_RIGHT:
    #             return 0
    #         case MotorLocation.FRONT_LEFT | MotorLocation.FRONT_RIGHT:
    #             return self.pitch
    #
    #     raise InvalidArgumentException(f'Motor location {location} does not exist!')
    #
    # def get_motor_roll(self, location):
    #     match location:
    #         case MotorLocation.FRONT_RIGHT | MotorLocation.BACK_RIGHT:
    #             return 0
    #         case MotorLocation.FRONT_LEFT | MotorLocation.BACK_LEFT:
    #             return self.roll
    #
    #     raise InvalidArgumentException(f'Motor location {location} does not exist!')
    #
    # def calc_motor_speed(self, location):
    #     ypr_multiplier = (self.get_motor_yaw(location) + self.get_motor_pitch(location) +
    #                       self.get_motor_roll(location)) / 1.5
    #     return self.throttle * ypr_multiplier

    def info(self):
        return f'motor speeds (1/2/3/4): ' \
               f'{self.motors[MotorLocation.BACK_RIGHT].speed}/' \
               f'{self.motors[MotorLocation.FRONT_RIGHT].speed}/' \
               f'{self.motors[MotorLocation.BACK_LEFT].speed}/' \
               f'{self.motors[MotorLocation.FRONT_LEFT].speed}'

    def stop(self):
        for motor in self.motors.values():
            motor.speed = 0

        self.pi.stop()
