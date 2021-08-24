from flask import request, jsonify, make_response
from sqlalchemy import exc
from models.questions import Questions, question_schema, questions_schema
from models.users import Users, user_schema, users_schema
from models.matches import Matches, match_schema, matches_schema
from models.__init__ import app, db


@app.route('/add_question', methods=['POST'])
def add_question():
    try:
        info = request.json
        new_question = Questions(info)
        new_question.save()
    except ValueError:
        return "Question with invalid format.", 400
    except TypeError:
        return "There is no body on the request  or it is incorrect.", 400
    except exc.IntegrityError:
        return "Question already in the database.", 403
    return "Question successfully added.", 200


@app.route('/get_questions', methods=['GET'])
def get_questions():
    questions = Questions.query.all()
    questions = questions_schema.dump(questions)
    return jsonify(questions)


@app.route('/get_question', methods={'GET'})
def get_question():
    try:
        question_id = request.args.get('id')
        question = Questions.query.filter_by(id=question_id).first()
        question = question_schema.dump(question)
        if question:
            return question, 200
    except ValueError:
        return "There is no body on the request or it is incorrect.", 400
    return "No question has such id.", 404


@app.route('/create_user', methods=['POST'])
def create_user():
    try:
        info = request.json
        new_user = Users(info)
        new_user.save()
    except TypeError:
        return "There is no body on the request or it is incorrect.", 400
    except exc.IntegrityError:
        return "User already exists.", 403
    return "User succesfully created.", 200


@app.route('/login', methods=['POST'])
def login():
    try:
        info = request.json
        user = Users(info)
        info = request.json
        if user.check():
            return "Log in successful.", 200
    except TypeError:
        return "There is no body on the request or it is incorrect.", 400
    return "Incorrect password or username.", 401


@app.route('/create_match', methods=['GET'])
def create_match():
    try:
        match = Matches()
        match.save()
    except exc.IntegrityError:
        return "An error occurred, please try again.", 500
    return {"match_id": match.id}, 200


@app.route('/add_score', methods=['POST'])
def add_score():
    try:
        info = request.json
        match = Matches.query.filter_by(id=info['id']).first()
        if match:
            match.add_score(info)
            return "Score added succesfully", 200
    except TypeError:
        return "There is no body on the request or it is incorrect.", 400
    except KeyError:
        return "The body has an incorrect format.", 400
    return "Invalid match.", 400


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
