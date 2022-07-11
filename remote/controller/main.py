import asyncio
import sys
import os
import websockets

from remote.controller.joy import Joy
from exceptions.exceptions import InvalidArgumentException
from remote.exceptions.exceptions import ButtonNotFoundException, JoyNotFoundException, GamepadNotFoundException
from gamepad import Gamepad


myDir = os.getcwd()
sys.path.append(myDir)

gamepad = Gamepad()
gamepad.connect(product_string='Xbox Wireless Controller')

PIQUAD_IP = '127.0.0.1'
uri = f'ws://{PIQUAD_IP}:8000'


def get_commands():
    try:
        data = gamepad.get_joy(Joy.LEFT_STICK_VERTICAL)
        return {'throttle': data}
    except ButtonNotFoundException and JoyNotFoundException and GamepadNotFoundException and InvalidArgumentException:
        return None
    except KeyboardInterrupt:
        exit(0)


async def websocket_send():
    async with websockets.connect(uri) as websocket:
        while True:
            data = await asyncio.get_event_loop().run_in_executor(None, get_commands)

            for command, value in data.items():
                await websocket.send(f'{command}:{value}')

asyncio.new_event_loop().run_until_complete(websocket_send())
