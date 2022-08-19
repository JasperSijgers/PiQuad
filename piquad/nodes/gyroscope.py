from threading import Thread

from sensors.mpu_6050 import Mpu6050


def mult_arr(a, mult):
    result = []

    for i in range(len(a)):
        if isinstance(a[i], list):
            temp = []
            for j in range(len(a[i])):
                temp.append(a[i][j] * mult)
            result.append(temp)
        else:
            result.append(a[i] * mult)

    return result


def sum_arr(a, b):
    result = []

    for i in range(len(a)):
        if isinstance(a[i], list):
            temp = []
            for j in range(len(a[i])):
                temp.append(a[i][j] + b[i][j])
            result.append(temp)
        else:
            result.append(a[i] + b[i])

    return result


class Gyroscope(Thread):

    def __init__(self, bus):
        super().__init__()

        self.publisher = bus.create_publisher(list, 'sensor-data', 10)
        self.mpu_6050 = Mpu6050()

        reading = self.mpu_6050.get_data()
        self.zero_point = mult_arr(reading, -1)

    def run(self):
        while True:
            sensor_data = self.mpu_6050.get_data()
            transformed = sum_arr(sensor_data, self.zero_point)
            self.publisher.send_message(transformed)
