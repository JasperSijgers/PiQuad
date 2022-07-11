from queue import Queue
from threading import Thread


class Topic(Thread):

    def __init__(self, msg_type, name, queue_size):
        super().__init__()

        self.msg_type = msg_type
        self.name = name
        self.queue_size = queue_size

        self.queue = Queue(maxsize=queue_size)
        self.subscribers = []

    def run(self):
        while True:
            msg = self.queue.get()

            for subscriber in self.subscribers:
                subscriber.send_message(msg)

    def add_subscriber(self, subscriber):
        self.subscribers.append(subscriber)

    def send_message(self, msg):
        if type(msg) is not self.msg_type:
            return

        if self.queue.full():
            self.queue.get(False)

        self.queue.put(msg)
