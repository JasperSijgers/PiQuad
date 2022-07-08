from controller.mappings.base_mapping import BaseMapping
from exceptions.exceptions import InvalidArgumentException


class JoyMapping(BaseMapping):

    def __init__(self, index=None, inverse=False, offset=0):
        super().__init__(index, inverse)
        self.offset = offset

    def get_offset(self):
        return self.offset

    def set_offset(self, offset):
        if not isinstance(offset, int):
            raise InvalidArgumentException('Offset must be an integer')

        self.offset = offset
