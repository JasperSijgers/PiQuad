from threading import Thread
from queue import Queue


class Subscriber(Thread):

    def __init__(self, callback_function, queue_size):
        super().__init__()
        self.queue = Queue(queue_size)
        self.callback = callback_function

    def run(self):
        while True:
            self.callback(self.queue.get())

    def send_message(self, msg):
        if self.queue.full():
            self.queue.get(False)

        self.queue.put(msg)
