from pynput.keyboard import Key, Listener, Controller
import logging
import websockets
import asyncio

PIQUAD_IP = '127.0.0.1'

class RemoteControl:

    def __init__(self):
        logging.basicConfig(level=logging.INFO)
        self.logger = logging.getLogger('piquad-remote')
        self.keyboard = Controller()

    def handle_key_pressed(self, key):
        if key in KEY_MAPPING.values():
            uri = f'ws://{PIQUAD_IP}:8000'
            message = list(KEY_MAPPING.keys())[list(KEY_MAPPING.values()).index(key)]

            try:
                asyncio.new_event_loop().run_until_complete(
                    self.send_message(uri, message)
                )
            except ConnectionError:
                self.logger.error(f'Unable to connect to: {uri}')

    def on_press(self, key):
        key_pressed = key

        if hasattr(key, 'char'):
            key_pressed = key.char

        self.logger.debug(f'Key pressed: {key_pressed}')
        if key_pressed == 'q':
            self.listener.stop()
            return False
        self.handle_key_pressed(key_pressed)

    def listen(self):
        with Listener(
            on_press=self.on_press,
            suppress=True
        ) as self.listener:
            self.listener.join()

    async def send_message(self, uri, message):
        async with websockets.connect(uri) as websocket:
            await websocket.send(message)
            # msg = await websocket.recv()
            # self.logger.info(f'Received message: {msg}')


RemoteControl().listen()
