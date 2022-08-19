import unittest

from exceptions import InvalidArgumentException
from mock_pigpio import MockPiGpio
from motor.motor import Motor

MOCK_PI_GPIO = MockPiGpio()


class MotorTests(unittest.TestCase):

    def test_create_motor(self):
        motor = Motor(MOCK_PI_GPIO, 1)
        self.assertTrue(type(motor) == Motor)

    def test_create_motor_invalid_gpio(self):
        self.assertRaises(InvalidArgumentException, Motor, MOCK_PI_GPIO, -1)
        self.assertRaises(InvalidArgumentException, Motor, MOCK_PI_GPIO, 32)

    def test_motor_turn_off(self):
        motor = Motor(MOCK_PI_GPIO, 1)

        motor.speed = 0
        self.assertEqual(motor.speed, 0)

    def test_motor_lower_speed_limit(self):
        motor = Motor(MOCK_PI_GPIO, 1)

        motor.speed = 1000
        self.assertEqual(motor.speed, 1000)

        motor.speed = 999
        self.assertEqual(motor.speed, 1000)

    def test_motor_upper_speed_limit(self):
        motor = Motor(MOCK_PI_GPIO, 1)

        motor.speed = 2000
        self.assertEqual(motor.speed, 2000)

        motor.speed = 2001
        self.assertEqual(motor.speed, 2000)
