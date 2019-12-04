from django.http import JsonResponse
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from django.shortcuts import render
import codecs
import sys

class Message:

    flag = 0

    msg = ""

    def __init__(self):
        print("msg init")

    @classmethod
    def append_msg(cls, msg):
        cls.msg += msg
        return cls.msg

    @classmethod
    def get_msg(cls):
        res = cls.msg
        cls.msg = ""
        return res


@csrf_exempt
def input_msg(request):

    res = ""
    # with codecs.open("index.html", "r", "utf-8") as file:
    #     for line in file.readlines():
    #         res += line
    # print(res)

    if request.method == 'POST':
        msg = request.POST["text"]

        print("received: " + msg)
        with codecs.open("C:\\Users\\jsjtx\\Desktop\\message.txt", 'w', 'utf-8') as file:
            file.write(msg)
            # file.write(str(Message.flag))
            # Message.flag = 1 - Message.flag
    return render(request, 'index.html')


@csrf_exempt
def get_msg(request):
    # res = {"msg": Message.get_msg()}
    return HttpResponse(Message.get_msg())
