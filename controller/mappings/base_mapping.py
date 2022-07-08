from exceptions.exceptions import InvalidArgumentException


class BaseMapping:

    def __init__(self, index=None, inverse=False):
        self.index = index
        self.inverse = inverse

    def get_index(self):
        return self.index

    def get_inverse(self):
        return self.inverse

    def set_index(self, index):
        if 0 < index < 16:
            raise InvalidArgumentException('Index out of bounds')

        self.index = index

    def set_inverse(self, inverse):
        if not isinstance(inverse, bool):
            raise InvalidArgumentException('Inverse must be a boolean')

        self.inverse = inverse
