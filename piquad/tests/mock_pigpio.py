class MockPiGpio:

    def set_servo_pulsewidth(self, user_gpio, pulsewidth):
        if user_gpio < 0 or user_gpio > 31:
            raise Exception("Invalid value provided for user_gpio")

        if pulsewidth != 0:
            if pulsewidth < 500 or pulsewidth > 2500:
                raise Exception("Invalid value provided for pulsewidth")

    def stop(self):
        return
