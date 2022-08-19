import time
import unittest

from messaging.message_bus import MessageBus
from messaging.topic import Topic

TOPIC_NAME = "some_topic_name"
SAMPLE_MESSAGE = "This is a message."

MESSAGES = []


def callback(message):
    MESSAGES.append(message)


class MessagingTests(unittest.TestCase):

    def test_create_message_bus(self):
        bus = MessageBus()
        self.assertEqual(bus.topics, {})

    def test_create_topic(self):
        bus = MessageBus()

        bus.ensure_topic(str, TOPIC_NAME, 10)
        self.assertTrue(bus.topic_exists(TOPIC_NAME))
        self.assertTrue(bus.topic_matches(str, TOPIC_NAME, 10))
        self.assertIsNotNone(bus.get_topic_if_exists(TOPIC_NAME))

    def test_create_publisher(self):
        bus = MessageBus()
        publisher = bus.create_publisher(str, TOPIC_NAME, 10)
        topic = bus.get_topic_if_exists(TOPIC_NAME)

        self.assertIsNotNone(topic)
        self.assertTrue(type(topic) == Topic)

        publisher.send_message(SAMPLE_MESSAGE)
        self.assertEqual(topic.queue.get(False), SAMPLE_MESSAGE)

    def test_create_subscriber(self):
        bus = MessageBus()
        publisher = bus.create_publisher(str, TOPIC_NAME, 10)
        bus.create_subscriber(str, TOPIC_NAME, callback, 10)

        publisher.send_message(SAMPLE_MESSAGE)
        time.sleep(.1)
        self.assertTrue(len(MESSAGES) == 1)
        self.assertEqual(MESSAGES[0], SAMPLE_MESSAGE)
