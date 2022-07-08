from topic import Topic
from publisher import Publisher
from subscriber import Subscriber


class MessageBus:

    def __init__(self):
        self.topics = {}

    def ensure_topic(self, msg_type, name, queue_size):
        if name not in self.topics:
            self.topics[name] = Topic(msg_type, name, queue_size)
            self.topics[name].start()

    def get_ensure_topic(self, msg_type, name, queue_size):
        self.ensure_topic(msg_type, name, queue_size)
        return self.topics[name]

    def get_topic_if_exists(self, name):
        if name in self.topics:
            return self.topics[name]

        return None

    def create_publisher(self, msg_type, name, queue_size=10):
        topic = self.get_ensure_topic(msg_type, name, queue_size)
        return Publisher(topic)

    def create_subscriber(self, msg_type, name, callback_function, queue_size=10):
        topic = self.get_ensure_topic(msg_type, name, queue_size)
        subscriber = Subscriber(callback_function, queue_size)
        subscriber.start()
        topic.add_subscriber(subscriber)
