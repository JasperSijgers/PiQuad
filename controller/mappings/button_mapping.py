from controller.mappings.base_mapping import BaseMapping
from exceptions.exceptions import InvalidArgumentException


class ButtonMapping(BaseMapping):

    def __init__(self, index=None, inverse=False, index_values=None):
        super().__init__(index, inverse)
        self.index_values = index_values

    def get_index_values(self):
        return self.index_values

    def set_index_values(self, index_values):
        if not isinstance(index_values, list):
            raise InvalidArgumentException('index_values must be a list')

        self.index_values = index_values
