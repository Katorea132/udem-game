from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_marshmallow import Marshmallow
import json


app = Flask(__name__)
with open('./config.json', 'r') as f:
    data = json.load(f)
    app.config['SQLALCHEMY_DATABASE_URI'] = f'{data["database_engine"]}+\
{data["database_driver"]}://{data["username"]}:{data["password"]}@\
{data["server"]}/game?charset=utf8mb4'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

db = SQLAlchemy(app)
ma = Marshmallow(app)
