"""
Everything regarding the users objects is here.
"""
from enum import unique
from . import db
from . import ma
from sqlalchemy.dialects.mysql import JSON
import hashlib
import base64


class Users(db.Model):
    """
    This class contains the users model, here lies the information of
    a given user.
    """
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(44), nullable=False)
    password = db.Column(db.String(44), nullable=False)
    unique_token = db.Column(db.String(44), nullable=False, unique=True)

    def save(self):
        """
        Commits changes in the object to the database.

        Returns:
            dict: the modified object.
        """
        db.session.add(self)
        db.session.commit()
        return self

    def check(self):
        """
        Checks if the username and password in the object
        correspond to any of those in the database.

        Returns:
            bool: True if it does, False if it doesnt.
        """
        user_token = self.query.filter_by(
            unique_token=self.unique_token).first()
        if user_token:
            return True
        return False

    def __init__(self, info):
        """
        Initializes a user.

        Args:
            info (dict): Contains the relevant information.
            This is the expected structure:
            {
                "username": "user name",
                "password": "SHA256 of the raw password as base64"
            }
        """
        self.username = info['username']  # Sin hash
        self.password = base64.b64encode(
            hashlib.sha256(
                bytes(
                    f"{info['password']}",
                    encoding='utf-8')).digest())
# Hash SHA256 en base 64 de password base 64 sha256
        self.unique_token = base64.b64encode(
            hashlib. sha256(
                bytes(
                    f"{self.username}{self.password}",
                    encoding='utf-8')).digest())

    def __repr__(self):
        return f"{self.id}"


class UsersSchema(ma.SQLAlchemyAutoSchema):
    class Meta:
        fields = ('id', 'username', 'password')


db.create_all()
user_schema = UsersSchema()
users_schema = UsersSchema(many=True)
