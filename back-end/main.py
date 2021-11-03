from flask import request, jsonify, make_response
from sqlalchemy import exc
from models.questions import Questions, question_schema, questions_schema
from models.users import Users, user_schema, users_schema
from models.matches import Matches, match_schema, matches_schema
from models.__init__ import app, db
from sqlalchemy.orm import close_all_sessions
from auxiliar_modules.auxi import dict_normalizer, scorizer


@app.route('/add_question', methods=['POST'])
def add_question():
    try:
        info = request.json
        new_question = Questions(info)
        new_question.save()
    except ValueError:
        return "Question with invalid format.", 400
    except TypeError:
        return "There is no body on the request or it is incorrect.", 400
    except exc.IntegrityError:
        return "Question already in the database.", 403
    return "Question successfully added.", 200


@app.route('/get_questions', methods=['GET'])
def get_questions():
    ret = ()
    try:
        questions_id = request.args.get('id')
        questions = Matches.query.filter_by(id=questions_id).first()
        questions = match_schema.dump(questions)
        if questions:
            ret = questions['questions_id'], 200
        else:
            ret = "No match has such id.", 404
    except ValueError:
        ret = "There is no query string on the request or it is incorrect.", 400
    finally:
        close_all_sessions()
    return ret


@app.route('/get_question', methods={'GET'})
def get_question():
    ret = []
    try:
        for question_id in request.args.get('id').split():
            question = Questions.query.filter_by(id=question_id).first()
            question = question_schema.dump(question)
            if question:
                ret.append(dict_normalizer(question))
            else:
                return "No question has such id.", 404
    except ValueError:
        ret = "There is no query string on the request or it is incorrect.", 400
    finally:
        close_all_sessions()
    return jsonify(ret), 200


@app.route('/create_user', methods=['POST'])
def create_user():
    ret = ()
    try:
        info = request.json
        new_user = Users(info)
        new_user.save()
        ret = "User succesfully created.", 200
    except TypeError:
        ret = ("There is no body on the request or it is incorrect.", 400)
    except exc.IntegrityError:
        ret = ("User already exists.", 403)
    finally:
        close_all_sessions()
    return ret


@app.route('/login', methods=['POST'])
def login():
    ret = ()
    try:
        info = request.json
        user = Users(info)
        info = request.json
        if user.check():
            ret = "Log in successful.", 200
        else:
            ret = "Incorrect password or username.", 401
    except TypeError:
        ret = "There is no body on the request or it is incorrect.", 400
    finally:
        close_all_sessions()
    return ret


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

@app.route('/get_score', methods=['GET'])
def get_scores():
    try:
        id = request.args.get('id')
        match = Matches.query.filter_by(id=id).first()
        if match:
            match = match_schema.dump(match)
            return scorizer(match), 200
    except TypeError:
        return "There is no body on the request or it is incorrect.", 400
    except KeyError:
        return "The body has an incorrect format.", 400
    return "Invalid match.", 400



if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
