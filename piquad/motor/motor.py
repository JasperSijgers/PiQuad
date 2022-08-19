import logging

from traitlets import HasTraits, Int, observe, validate

from exceptions import InvalidArgumentException


class Motor(HasTraits):
    speed = Int()

    def __init__(self, pi, gpio, *args, **kwargs):
        super().__init__(*args, **kwargs)

        if gpio < 0 or gpio > 31:
            raise InvalidArgumentException("GPIO out of range [0 ... 31]")

        self.pi = pi
        self.gpio = gpio
        self.logger = logging.getLogger('piquad')

    @validate('speed')
    def _clip_speed(self, proposal):
        if proposal['value'] > 2000:
            return 2000
        elif proposal['value'] != 0 and proposal['value'] < 1000:
            return 1000
        else:
            return proposal['value']

    @observe('speed')
    def _on_speed(self, change):
        self.logger.debug("set motor {0} speed to {1}".format(self.gpio, change['new']))
        self.pi.set_servo_pulsewidth(int(self.gpio), int(change['new']))
