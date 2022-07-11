import websockets
import asyncio
import logging
from threading import Thread


class RemoteReceiver(Thread):

    def __init__(self, bus):
        super().__init__()
        self.logger = logging.getLogger('piquad')

        self.mc_publisher = bus.create_publisher(str, 'motor-controller', 10)

    def run(self):
        asyncio.set_event_loop(asyncio.new_event_loop())
        asyncio.get_event_loop().run_until_complete(
            websockets.serve(self.incoming_message, '0.0.0.0', 8000)
        )
        asyncio.get_event_loop().run_forever()

    async def incoming_message(self, websocket, path):
        while True:
            try:
                message = await websocket.recv()
                self.logger.debug(f'Command received from websocket: {message}')

                split_msg = message.split(':')

                if split_msg[0] in ['throttle', 'yaw', 'pitch', 'roll']:
                    self.mc_publisher.send_message(message)

                # TODO: Handle a button press
            except websockets.ConnectionClosed:
                break
