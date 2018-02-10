import urllib.request

failed = 0
hosts = ['google','yahoo','facebook','instagram']

for host in hosts:
	url = 'http://' + host + '.com'
	req = urllib.request.Request(url)

	try:
		print('testing ' + url)
		urllib.request.urlopen(req)
	except urllib.error.HTTPError as e:
		failed += 1

print(str(failed) + ' host(s) failed')
