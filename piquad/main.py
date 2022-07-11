import os
import logging
from dotenv import load_dotenv
from messaging.message_bus import MessageBus
from nodes.motor_controller import MotorController
from nodes.remote_receiver import RemoteReceiver
from nodes.gyroscope import Gyroscope
from nodes.information_publisher import InfoPublisher


def setup():
    # Load in the environment variables
    base_dir = os.path.dirname(os.path.abspath(__file__))
    load_dotenv(dotenv_path=f'{base_dir}/.env')

    # Set up the logger
    logging.basicConfig(level=logging.DEBUG)

    # Set up the message bus
    bus = MessageBus()

    # Start up the motor controller
    MotorController(bus)

    # Start up the Gyroscope & Accelerometer
    gyro = Gyroscope(bus)
    gyro.start()

    # Start up the command receiver
    receiver = RemoteReceiver(bus)
    receiver.start()

    # Start up the information publisher
    publisher = InfoPublisher(bus)
    publisher.start()


if __name__ == "__main__":
    setup()
