import asyncio
from threading import Thread

import websockets


class InfoPublisher(Thread):

    def __init__(self, bus):
        super().__init__()

        self.subscribers = []

        bus.create_subscriber(str, 'information-publisher', self.handle_new_subscriber, 10)
        bus.create_subscriber(str, 'motor-controller', self.handle_sensor_data, 10)
        bus.create_subscriber(str, 'sensor-data', self.handle_sensor_data, 10)

    def handle_new_subscriber(self, msg):
        self.add_subscriber(f'ws://{msg}:8000')

    def handle_sensor_data(self, msg):
        self.publish_data(msg)

    def add_subscriber(self, uri):
        if uri not in self.subscribers:
            self.subscribers.append(uri)

    def remove_subscriber(self, uri):
        if uri in self.subscribers:
            self.subscribers.remove(uri)

    def publish_data(self, message):
        for uri in self.subscribers:
            try:
                asyncio.new_event_loop().run_until_complete(
                    self.send_message(uri, message)
                )
            except ConnectionError:
                self.remove_subscriber(uri)

    async def send_message(self, uri, message):
        async with websockets.connect(uri) as websocket:
            await websocket.send(str(message))
