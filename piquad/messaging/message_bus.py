from exceptions import TopicParametersMismatchException
from .publisher import Publisher
from .subscriber import Subscriber
from .topic import Topic


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

    def topic_exists(self, name):
        return name in self.topics

    def topic_matches(self, msg_type, name, queue_size):
        if not self.topic_exists(name):
            return False

        return self.topics[name].msg_type == msg_type and self.topics[name].queue_size == queue_size

    def get_topic_if_exists(self, name):
        if name in self.topics:
            return self.topics[name]

        return None

    def create_publisher(self, msg_type, name, queue_size=10):
        if self.topic_exists(name) and not self.topic_matches(msg_type, name, queue_size):
            raise TopicParametersMismatchException("Topic already exists, but its parameters don't match the provided.")

        topic = self.get_ensure_topic(msg_type, name, queue_size)
        return Publisher(topic)

    def create_subscriber(self, msg_type, name, callback_function, queue_size=10):
        topic = self.get_ensure_topic(msg_type, name, queue_size)
        subscriber = Subscriber(callback_function, queue_size)
        subscriber.start()
        topic.add_subscriber(subscriber)
