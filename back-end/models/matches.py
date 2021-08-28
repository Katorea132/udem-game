"""
Everything regarding the matches objects is here.
"""
from . import db
from . import ma
from .questions import Questions, question_schema
from .users import Users, user_schema
from sqlalchemy.dialects.mysql import JSON
from sqlalchemy.sql.expression import func
from sqlalchemy.orm.attributes import flag_modified


class Matches(db.Model):
    """
    This class contains the matches model, here lies the information
    of which questions a given match has
    """
    id = db.Column(db.String(100), primary_key=True)
    score = db.Column(JSON)
    questions_id = db.Column(db.String(100), nullable=False)

    def save(self):
        """
        Commits changes in the object to the database.

        Returns:
            dict: The modified object.
        """
        db.session.add(self)
        db.session.commit()
        db.session.close()
        return self

    def random_questions_generator(self, sample_num):
        """
        Generates a list of question ids from the question database
        in random order.

        Args:
            sample_num (int): Number of questions to pulls.
        Returns:
            list: list of question ids in a random order.
        """
        if not sample_num:
            return []
        return Questions.query.order_by(func.random()).limit(sample_num).all()

    def add_score(self, info):
        """
        Adds the corresponding score to the corresponding user
        and saves the changes in the database.

        Args:
            info (dict): Contains the relevant information to properly
            add the score.
            This is the expected structure:
            {
                "id": "match id",
                "score_info": {
                    "question": "question id",
                    "answer": "answer given to the question",
                    "time": int - remaining time (base taken as 100)
                },
                "token": "b9UrOSumTOPhnaQClmFUI5DPYwcsV8QETWZrTN23FLU="
            }
        """
        score = self.score_calculator(info['score_info'])
        user = Users.query.filter_by(unique_token=info['token']).first()
        user = user_schema.dump(user)
        if user['username'] in self.score["scores"]:
            self.score["scores"][user['username']] += score
        else:
            self.score["scores"][user['username']] = score
        flag_modified(self, "score")
        self.save()

    def score_calculator(self, score_info):
        """
        Determines the amount of points to give, considering
        the difficulty associated with the question (1 is standard, less is
        easier, and more is harder) and the time it took the person to answer.

        Args:
            score_info (dict): Dictionary with the following required values.
            {
                "question": "question id",
                "answer": "answer given to the question",
                "time": int - remaining time (base taken as 100)
            }

        Returns:
            int: amount of points to give.
        """
        base_time = 100
        question = Questions.query.filter_by(id=score_info['question']).first()
        question = question_schema.dump(question)
        if question['answers'][score_info['answer']]:
            score = 100 * question['difficulty'] / \
                (score_info['time'] / base_time)
        else:
            score = 0
        return score

    def __init__(self):
        """
        Initialized the match, the id are the question ids joined
        together in a string.
        """
        questions = self.random_questions_generator(20)
        self.id = "".join([str(question) for question in questions])
        self.score = {"scores": {}}
        self.questions_id = " ".join([str(question) for question in questions])

    def __repr__(self):
        return f"{self.id}"


class MatchesSchema(ma.SQLAlchemyAutoSchema):
    class Meta:
        fields = ('id', 'score', 'questions_id')


db.create_all()
match_schema = MatchesSchema()
matches_schema = MatchesSchema(many=True)
