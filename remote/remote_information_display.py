import os
import websockets
import asyncio
from ast import literal_eval


async def incoming_message(websocket, path):
    async for message in websocket:
        try:
            arr = literal_eval(message)
            print_values(arr)
        except ValueError:
            print(message)


def print_values(values):
    gyro_vals = [round(a, 2) for a in values[1]]
    acc_vals = [round(a, 2) for a in values[3]]
    x_rot = round(values[4], 2)
    y_rot = round(values[5], 2)

    os.system('clear')
    print('{:<20}{:<20}{:<20}{:<20}'.format('Gyroscope:', gyro_vals[0], gyro_vals[1], gyro_vals[2]))
    print('{:<20}{:<20}{:<20}{:<20}'.format('Accelerometer:', acc_vals[0], acc_vals[1], acc_vals[2]))
    print('{:<20}{:<20}'.format('X-Rotation:', x_rot))
    print('{:<20}{:<20}'.format('Y-Rotation:', y_rot))


if __name__ == '__main__':
    asyncio.get_event_loop().run_until_complete(
        websockets.serve(incoming_message, '0.0.0.0', 8000)
    )
    asyncio.get_event_loop().run_forever()
