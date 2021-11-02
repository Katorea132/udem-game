
def dict_normalizer(ugly_dictionary):
    print(ugly_dictionary)
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