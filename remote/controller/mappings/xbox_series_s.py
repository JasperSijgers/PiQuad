from remote.controller.button import Button
from remote.controller.joy import Joy
from remote.controller.mappings.button_mapping import ButtonMapping
from remote.controller.mappings.joy_mapping import JoyMapping


class XboxSeriesSMapping:

    def __init__(self):
        self.button_mappings = {}
        self.joy_mappings = {}

        self.setup_mappings()

    def get_button_mappings(self):
        return self.button_mappings

    def get_joy_mappings(self):
        return self.joy_mappings

    def get_mappings(self):
        return {**self.button_mappings, **self.joy_mappings}

    def add_button_mapping(self, button, index, index_values, inverse=False):
        self.button_mappings[button.value] = ButtonMapping(index, inverse, index_values)

    def add_joy_mapping(self, joy, index, inverse=False, value_range=None):
        self.joy_mappings[joy.value] = JoyMapping(index, inverse, value_range)

    def setup_mappings(self):
        self.add_button_mapping(Button.A, 14, [1])
        self.add_button_mapping(Button.B, 14, [2])
        self.add_button_mapping(Button.X, 14, [8])
        self.add_button_mapping(Button.Y, 14, [16])
        self.add_button_mapping(Button.LEFT_BUMPER, 14, [64])
        self.add_button_mapping(Button.RIGHT_BUMPER, 14, [128])
        self.add_button_mapping(Button.MENU, 15, [8])
        self.add_button_mapping(Button.SHARE, 16, [1])
        self.add_button_mapping(Button.VIEW, 15, [4])
        self.add_button_mapping(Button.XBOX, 15, [16])
        self.add_button_mapping(Button.D_PAD_TOP, 13, [8, 1, 2])
        self.add_button_mapping(Button.D_PAD_BOTTOM, 13, [4, 5, 6])
        self.add_button_mapping(Button.D_PAD_LEFT, 13, [6, 7, 8])
        self.add_button_mapping(Button.D_PAD_RIGHT, 13, [2, 3, 4])

        self.add_joy_mapping(Joy.LEFT_STICK_HORIZONTAL, 2, False, [0, 255])
        self.add_joy_mapping(Joy.LEFT_STICK_VERTICAL, 4, True, [0, 255])
        self.add_joy_mapping(Joy.RIGHT_STICK_HORIZONTAL, 6, False, [0, 255])
        self.add_joy_mapping(Joy.RIGHT_STICK_VERTICAL, 8, True, [0, 255])
        self.add_joy_mapping(Joy.LEFT_TRIGGER, 9)
        self.add_joy_mapping(Joy.RIGHT_TRIGGER, 11)
