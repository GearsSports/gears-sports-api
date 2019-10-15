<?php

require_once __DIR__ . '/../vendor/autoload.php';

use PHPUnit\Framework\TestCase;

use Gears\Proto\Api\V1\TokenRequest;
use Gears\Proto\Api\V1\AuthServiceClient;
use Gears\Proto\Api\V1\PlayerServiceClient;
use Gears\Proto\Api\V1\CaptureServiceClient;
use Gears\Proto\Api\V1\ListPlayersRequest;
use Gears\Proto\Api\V1\ListCapturesRequest;
use Gears\Proto\Capture\Capture;
use Gears\Proto\Player\Player;
use Gears\Proto\Server\CaptureInfo;

class ServerApiAccessTest extends TestCase {

    private $testFolder = __DIR__."/../../data/";
    private $url = 'devrpc.gearssports.com:443';

    private $tokenRequest;
    private $credentials;
    private $channel;

    protected function setUp()
    {
        $file = $this->testFolder . "TokenRequest.json";
        $this->assertTrue(file_exists($file));

        $this->tokenRequest = new TokenRequest();
        $this->tokenRequest->mergeFromJsonString(file_get_contents($file));

        $certFile = $this->testFolder . 'public.pem';
        $this->assertTrue(file_exists($certFile));

        $this->credentials = \Grpc\ChannelCredentials::createSsl(file_get_contents($certFile));
        $this->channel = new \Grpc\Channel($this->url,['credentials'=>$this->credentials]);
    }

    public function testAuthenticate()
    {
        $authClient = new AuthServiceClient($this->url,['credentials'=>$this->credentials],$this->channel);
        list($token,$status) = $authClient->Token($this->tokenRequest)->wait();

        // A non-zero status code means an error occurred
        $this->assertEquals(0,$status->code);

        // If there is a non-zero status then the details string will be populated with information about the error
        $this->assertEmpty($status->details);

        // This is the auth token that you will need to access the other methods in our api
        $this->assertNotNull($token);

        // An auth token can be used until it expires. The field response.expires_in
        // represents the number of seconds the token is good for.
        $this->assertGreaterThan(0,$token->getExpiresIn());
    }

    public function testListPlayers()
    {
        $authClient = new AuthServiceClient($this->url,['credentials'=>$this->credentials],$this->channel);
        list($token,$status) = $authClient->Token($this->tokenRequest)->wait();

        $accessToken = $token->getAccessToken();

        $callOptions = $this->getServiceOptions($accessToken);
        $playerClient = new PlayerServiceClient($this->url,$callOptions,$this->channel);

        // There are numerous filters available but we won't be using them here
        // To use them:
        //    1) create a ListPlayersFilters object
        //    2) call various set methods on the object according to needs
        //    3) pass the object to $playerRequest->setFilters
        $playerRequest = new ListPlayersRequest();
        $playerRequest->setPerPage(2);

        list($playerResponse,$status) = $playerClient->list($playerRequest)->wait();

        // A non-zero status code means an error occurred
        $this->assertEquals(0,$status->code);

        // If there is a non-zero status then the details string will be populated with information about the error
        $this->assertEmpty($status->details);

        $this->assertNotNull($playerResponse);
        $this->assertEquals($playerRequest->getPerPage(),$playerResponse->getPerPage());

        $players = $playerResponse->getPlayers();
        $this->assertGreaterThan(0,$players->count());

        foreach($players as $player) {
            $this->assertInstanceOf(Player::class,$player);
            $this->assertNotNull($player->getId());
        }
    }

    public function testListCaptures()
    {
        $authClient = new AuthServiceClient($this->url,['credentials'=>$this->credentials],$this->channel);
        list($token,$status) = $authClient->Token($this->tokenRequest)->wait();

        $accessToken = $token->getAccessToken();

        $callOptions = $this->getServiceOptions($accessToken);
        $captureClient = new CaptureServiceClient($this->url,$callOptions,$this->channel);

        // There are numerous filters available but we won't be using them here
        // To use them:
        //    1) create a ListCapturesFilters object
        //    2) call various set methods on the object according to needs
        //    3) pass the object to $captureRequest->setFilters
        $captureRequest = new ListCapturesRequest();
        $captureRequest->setPerPage(2);

        list($captureResponse,$status) = $captureClient->list($captureRequest)->wait();

        // A non-zero status code means an error occurred
        $this->assertEquals(0,$status->code);

        // If there is a non-zero status then the details string will be populated with information about the error
        $this->assertEmpty($status->details);

        $this->assertNotNull($captureResponse);
        $this->assertEquals($captureRequest->getPerPage(),$captureResponse->getPerPage());

        $captures = $captureResponse->getCaptures();
        $this->assertGreaterThan(0,$captures->count());

        foreach($captures as $capture) {
            $this->assertInstanceOf(CaptureInfo::class,$capture);
            $this->assertNotNull($capture->getId());

            // A CaptureInfo instance only contains basic info about a capture.
            // If you want the full capture object you can download it using the url returned by captureInfo.getUrl().
            // The url returned by captureInfo.getUrl() should not be stored as it will expire a few minutes after it is generated.
            // This file is usually large and can take some time to deserialize - so only load it as needed
            $url = $capture->getUrl();
            $this->assertNotNull($url);

            $capture = new Capture();
            $capture->mergeFromString(file_get_contents($url));
            $this->assertNotNull($capture->getId());
        }

    }

    private function getServiceOptions($token)
    {
        // Once you have an access token, you need to add it to the headers
        $this->token = $token;
        return [
            'credentials' => $this->credentials,
            'update_metadata' => function($metaData){
                $metaData['authorization'] = ['Bearer ' . $this->token];
                $metaData['client-version'] = ['0.1'];
                return $metaData;
            }
        ];
    }
}