"""
Everything regarding the questions objects is here.
"""
from . import db
from . import ma
from sqlalchemy.dialects.mysql import JSON


class Questions(db.Model):
    """
    This class contains the matches model, here lies the information
    of which questions a given match has.
    """
    id = db.Column(db.Integer, primary_key=True)
    question = db.Column(db.String(150), nullable=False, unique=True)
    answers = db.Column(JSON, nullable=False)
    difficulty = db.Column(db.Integer(), nullable=False)

    def save(self):
        """
        Commits changes in the object to the database.

        Returns:
            dict: the modified object.
        """
        db.session.add(self)
        db.session.commit()
        return self

    def answers_checker(self, options):
        """
        Checks if the answers given have at least 1 correct answer
        and are formatted properly.

        Args:
            answers (dict): This will check if the structure for the
            given answers is correct. It must have AT LEAST 1 correct answer.

        Structure:
            {
                "option 1": True/False depending if it's a valid answer,
                "option 2": True/False,
                "option 3": True/False,
                "option_4": True/False
            }
        """
        if isinstance(options, dict) and len(options) == 4 and \
           (True in options.values()):
            return options
        else:
            raise ValueError('The given dictionary is invalid.')

    def __init__(self, info):
        """
        Initialized the question object.

        Args:
            info (dict): Contains the relevant information.
            This is the expected structure:
            {
                "question": "Is this a question?",
                "answers": {
                    "Yes": True,
                    "Nah": False,
                    "Los martes": False,
                    "Naruhodo ne *doesn't naruhodo at all*": False
                },
                "difficulty": 9999 - float - multiplier for the question.
            }
        """
        self.question = info['question']
        self.answers = self.answers_checker(info['answers'])
        self.difficulty = info['difficulty']

    def __repr__(self):
        return f"{self.id}"


class QuestionsSchema(ma.SQLAlchemyAutoSchema):
    class Meta:
        fields = ('id', 'question', 'answers', 'difficulty')


db.create_all()
question_schema = QuestionsSchema()
questions_schema = QuestionsSchema(many=True)
