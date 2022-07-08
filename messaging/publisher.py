class Publisher:

    def __init__(self, topic):
        self.topic = topic

    def send_message(self, message):
        self.topic.send_message(message)
