import hid

from joy import Joy
from button import Button
from exceptions.exceptions import ButtonNotFoundException, JoyNotFoundException, GamepadNotFoundException, \
    InvalidArgumentException
from controller.mappings.xbox_series_s import XboxSeriesSMapping
from controller.mappings.joy_mapping import JoyMapping
from controller.mappings.button_mapping import ButtonMapping


class Gamepad:
    buttons = {}
    joys = {}

    def __init__(self):
        self.gamepad = None
        self.mappings = XboxSeriesSMapping().get_mappings()

        for button in Button:
            self.buttons[button.value] = 0

        for joy in Joy:
            self.joys[joy.value] = 0

    def get(self):
        self.read()
        return {**self.buttons, **self.joys}

    def get_buttons(self):
        self.read()
        return self.buttons

    def get_joys(self):
        self.read()
        return self.joys

    def get_button(self, button):
        self.read()

        if button.value not in self.buttons.keys():
            raise ButtonNotFoundException(f'Button with name {button} not found')

        return self.buttons[button.value]

    def get_joy(self, joy):
        self.read()

        if joy.value not in self.joys.keys():
            raise JoyNotFoundException(f'Joy with name {joy} not found')

        return self.joys[joy.value]

    def read(self, max_bytes=64):
        report = self.gamepad.read(max_bytes)

        if not report:
            return

        for k, v in self.mappings.items():
            if isinstance(v, JoyMapping):
                self.joys[k] = ((-1 if v.inverse else 1) * report[v.index]) + v.offset
            elif isinstance(v, ButtonMapping):
                self.buttons[k] = report[v.index] not in v.index_values if v.inverse else \
                    report[v.index] in v.index_values

    def connect(self, vendor_id=None, product_id=None, product_string=None):
        if vendor_id and product_id:
            self.gamepad = hid.device()
            self.gamepad.open(vendor_id, product_id)
            self.gamepad.set_nonblocking(True)
            return

        if product_string:
            for device in hid.enumerate():
                if device['product_string'] == product_string:
                    self.gamepad = hid.device()
                    self.gamepad.open(device['vendor_id'], device['product_id'])
                    self.gamepad.set_nonblocking(True)
                    break
            if not self.gamepad:
                raise GamepadNotFoundException(f'Could not connect to gamepad with product string: {product_string}')

        if not self.gamepad:
            raise InvalidArgumentException('Could not connect to a gamepad: either vendor_id and product_id or product_'
                                           'string must be provided')
