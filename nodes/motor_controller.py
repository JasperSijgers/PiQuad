import os
import time

import pigpio
import logging
from motors.motor import Motor
from motors.motor_location import MotorLocation

THROTTLE_MIN = os.environ.get("THROTTLE_MIN") or 1000
THROTTLE_MAX = os.environ.get("THROTTLE_MAX") or 2000


class MotorController:

    def __init__(self, bus):
        super().__init__()

        self.pi = pigpio.pi()
        self.logger = logging.getLogger('piquad')
        self.throttle = 1000

        self.motors = {MotorLocation.BACK_RIGHT: Motor(self.pi, os.environ.get("GPIO_PIN_M1") or 23),
                       MotorLocation.FRONT_RIGHT: Motor(self.pi, os.environ.get("GPIO_PIN_M2") or 24),
                       MotorLocation.BACK_LEFT: Motor(self.pi, os.environ.get("GPIO_PIN_M3") or 25),
                       MotorLocation.FRONT_LEFT: Motor(self.pi, os.environ.get("GPIO_PIN_M4") or 16)}

        bus.create_subscriber(str, 'motor-controller', self.handle_msg, 10)

    def handle_msg(self, msg):
        try:
            getattr(self, msg)()
        except AttributeError:
            self.logger.error(f"Function {msg}() not found in Controller class")

    def set_motor_speed(self, location, speed):
        if location == MotorLocation.ALL:
            for motor in self.motors.values():
                motor.speed = speed
        else:
            self.motors[location].speed = speed

        self.logger.info(self.info())

    def info(self):
        return f'motor speeds (1/2/3/4): (' \
               f'{self.motors[MotorLocation.BACK_RIGHT].speed}/' \
               f'{self.motors[MotorLocation.FRONT_RIGHT].speed}/' \
               f'{self.motors[MotorLocation.BACK_LEFT].speed}/' \
               f'{self.motors[MotorLocation.FRONT_LEFT].speed})'

    def increase_throttle(self):
        self.throttle += 50 if self.throttle <= (THROTTLE_MAX - 50) else THROTTLE_MAX
        self.set_motor_speed(MotorLocation.ALL, self.throttle)

    def decrease_throttle(self):
        self.throttle -= 50 if self.throttle >= (THROTTLE_MIN + 50) else THROTTLE_MIN
        self.set_motor_speed(MotorLocation.ALL, self.throttle)

    def stop(self):
        for motor in self.motors.values():
            motor.speed = 0

        self.pi.stop()
