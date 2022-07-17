

## Steps

[Request reader] => [Request Validator] => [Grant Type Handler] => [Response writer]


## Services

- Reader `IRequestParamReader`
- Validator
- Handler
- Writer

## Flows

### Device Code Flow

https://www.oauth.com/oauth2-servers/device-flow/token-request/

step 1:

device request :

```
POST https://example.com/device

client_id=myid

HTTP/1.1 200
Content-Type: application/json
Cache-Control: no-store

{
  "device_code": "NGU5OWFiNjQ5YmQwNGY3YTdmZTEyNzQ3YzQ1YSA",
  "user_code": "BDWD-HQPK",
  "verification_uri": "https://example.com/device",
  "interval": 5,
  "expires_in": 1800
}
```

device request :

```
POST /token HTTP/1.1
Content-type: application/x-www-form-urlencoded
 
grant_type=urn:ietf:params:oauth:grant-type:device_code&
client_id=a17c21ed&
device_code=NGU5OWFiNjQ5YmQwNGY3YTdmZTEyNzQ3YzQ1YSA
```
