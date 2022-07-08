import websockets
import asyncio
import logging
from threading import Thread


class CommandReceiver(Thread):

    def __init__(self, bus):
        super().__init__()
        self.logger = logging.getLogger('piquad')

        self.command_publisher = bus.create_publisher(str, 'motor-controller', 10)
        self.ip_publisher = bus.create_publisher(str, 'information-publisher', 10)

    def run(self):
        asyncio.set_event_loop(asyncio.new_event_loop())
        asyncio.get_event_loop().run_until_complete(
            websockets.serve(self.incoming_message, '0.0.0.0', 8000)
        )
        asyncio.get_event_loop().run_forever()

    async def incoming_message(self, websocket, path):
        async for message in websocket:
            self.logger.debug(f'Command received from websocket: {message}')
            self.command_publisher.send_message(str(message))
            self.ip_publisher.send_message(str(websocket.remote_address[0]))