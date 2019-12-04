from django.shortcuts import render


def toIndex(request):
	return render(request, 'index.html')
