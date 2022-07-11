from remote.controller.mappings.base_mapping import BaseMapping
from exceptions.exceptions import InvalidArgumentException


class JoyMapping(BaseMapping):

    def __init__(self, index=None, inverse=False, value_range=None):
        super().__init__(index, inverse)
        self.value_range = value_range or [0, 255]

    def get_value_range(self):
        return self.value_range

    def set_offset(self, value_range):
        if not isinstance(value_range, list) or len(value_range) != 2:
            raise InvalidArgumentException('value_range must be a list of length 2')

        self.value_range = value_range
