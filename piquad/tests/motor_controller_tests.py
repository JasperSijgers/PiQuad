import unittest

from exceptions import MotorControlException
from messaging.message_bus import MessageBus
from mock_pigpio import MockPiGpio
from nodes.motor_controller import MotorController

MOCK_PI_GPIO = MockPiGpio()
MESSAGE_BUS = MessageBus()


class MotorControllerTests(unittest.TestCase):

    def test_create_motor_controller(self):
        motor_controller = MotorController(MOCK_PI_GPIO, MESSAGE_BUS)
        self.assertTrue(type(motor_controller) == MotorController)

    def test_throttle_off(self):
        motor_controller = MotorController(MOCK_PI_GPIO, MESSAGE_BUS)

        motor_controller.handle_msg('throttle:0')
        self.assertEqual(motor_controller.throttle, 0)

        motor_controller.stop()
        self.assertEqual(motor_controller.throttle, 0)

    def test_throttle_lower_limit(self):
        motor_controller = MotorController(MOCK_PI_GPIO, MESSAGE_BUS)

        motor_controller.handle_msg('throttle:1000')
        self.assertEqual(motor_controller.throttle, 1000)

        self.assertRaises(MotorControlException, motor_controller.handle_msg, 'throttle:999')

    def test_throttle_upper_limit(self):
        motor_controller = MotorController(MOCK_PI_GPIO, MESSAGE_BUS)

        motor_controller.handle_msg('throttle:2000')
        self.assertEqual(motor_controller.throttle, 2000)

        self.assertRaises(MotorControlException, motor_controller.handle_msg, 'throttle:2001')
