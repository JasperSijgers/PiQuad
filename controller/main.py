from controller.joy import Joy
from controller.button import Button
from exceptions.exceptions import ButtonNotFoundException, JoyNotFoundException, GamepadNotFoundException, \
    InvalidArgumentException
from gamepad import Gamepad
import sys
import os

myDir = os.getcwd()
sys.path.append(myDir)

gamepad = Gamepad()
gamepad.connect(product_string='Xbox Wireless Controller')

while True:
    try:
        print(gamepad.get_joy(Joy.RIGHT_TRIGGER))
    except ButtonNotFoundException and JoyNotFoundException and GamepadNotFoundException and InvalidArgumentException:
        continue
    except KeyboardInterrupt:
        exit(0)
