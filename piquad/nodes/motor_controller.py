import logging
import os

from exceptions import MotorControlException
from motor.motor import Motor
from motor.motor_location import MotorLocation


def int_from_env(key):
    try:
        return int(os.environ.get(key))
    except TypeError:
        return None


THROTTLE_MIN = int_from_env('MIN_THROTTLE') or 1000
THROTTLE_MAX = int_from_env('MAX_THROTTLE') or 2000


class MotorController:

    def __init__(self, pi, bus):
        super().__init__()

        self.pi = pi
        self.logger = logging.getLogger('piquad')
        self.throttle = 0
        self.motors = {MotorLocation.BACK_RIGHT: Motor(self.pi, int_from_env('GPIO_PIN_M1') or 23),
                       MotorLocation.FRONT_RIGHT: Motor(self.pi, int_from_env('GPIO_PIN_M2') or 24),
                       MotorLocation.BACK_LEFT: Motor(self.pi, int_from_env('GPIO_PIN_M3') or 25),
                       MotorLocation.FRONT_LEFT: Motor(self.pi, int_from_env('GPIO_PIN_M4') or 16)}

        bus.create_subscriber(str, 'motor-controller', self.handle_msg, 10)

    def handle_msg(self, msg):
        operation, value = msg.split(':')

        if operation not in ['throttle']:
            raise MotorControlException(f'Invalid value for operation provided: {operation}')

        if operation == 'throttle':
            val = int(value)
            if val != 0 and (val < THROTTLE_MIN or val > THROTTLE_MAX):
                raise MotorControlException(f'Invalid value for throttle provided: {value}')

        setattr(self, operation, int(value))
        self.update_motor_speed()

    def update_motor_speed(self, location=MotorLocation.ALL):
        if location == MotorLocation.ALL:
            for location in self.motors.keys():
                self.update_motor_speed(location)
            return

        self.motors[location].speed = self.throttle

    def stop(self):
        for motor in self.motors.values():
            motor.speed = 0

        self.pi.stop()
