import math

import smbus

power_mgmt_1 = 0x6b
power_mgmt_2 = 0x6c


def read_byte(adr):
    return sm_bus.read_byte_data(address, adr)


def read_word(adr):
    high = sm_bus.read_byte_data(address, adr)
    low = sm_bus.read_byte_data(address, adr + 1)
    val = (high << 8) + low
    return val


def read_word_2c(adr):
    val = read_word(adr)
    if val >= 0x8000:
        return -((65535 - val) + 1)
    else:
        return val


def dist(a, b):
    return math.sqrt((a * a) + (b * b))


def get_y_rotation(accel_scaled):
    radians = math.atan2(accel_scaled[0], dist(accel_scaled[1], accel_scaled[2]))
    return -math.degrees(radians)


def get_x_rotation(accel_scaled):
    radians = math.atan2(accel_scaled[1], dist(accel_scaled[0], accel_scaled[2]))
    return math.degrees(radians)


sm_bus = smbus.SMBus(1)
address = 0x68

sm_bus.write_byte_data(address, power_mgmt_1, 0)


class Mpu6050:

    def get_data(self):
        gyro_xout = read_word_2c(0x43)
        gyro_yout = read_word_2c(0x45)
        gyro_zout = read_word_2c(0x47)

        gyro = [gyro_xout, gyro_yout, gyro_zout]
        gyro_scaled = [i / 131 for i in gyro]

        accel_xout = read_word_2c(0x3b)
        accel_yout = read_word_2c(0x3d)
        accel_zout = read_word_2c(0x3f)

        accel = [accel_xout, accel_yout, accel_zout]
        accel_scaled = [i / 16384.0 for i in accel]

        x_rot = get_x_rotation(accel_scaled)
        y_rot = get_y_rotation(accel_scaled)

        return [gyro, gyro_scaled, accel, accel_scaled, x_rot, y_rot]
