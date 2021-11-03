
def dict_normalizer(ugly_dictionary):
    pretty_dict = {
        "question": ugly_dictionary['question'],
        "answers": "",
        "difficulty": ugly_dictionary['difficulty'],
        "id": ugly_dictionary['id']
    }
    answers_text = ""
    for k, v in ugly_dictionary['answers'].items():
        answers_text += k + "/"
    
    pretty_dict['answers'] = answers_text[:-1]

    return pretty_dict

def scorizer(match):
    ret = ""
    for k, v in match['score']['scores'].items():
        ret += k + "\t" + str(v) + "\n"
    return ret


# //     {"questions":[
# //   {
# //     "answers": {
# //       "Los martes": false, 
# //       "Nah": false, 
# //       "Naruhodo ne *doesn't naruhodo at all": false, 
# //       "Yyyes": true
# //     }, 
# //     "difficulty": 9999, 
# //     "id": 11, 
# //     "question": "Are wwwwwwwwwwe there yet Mr. Krabs?"
# //   }, 