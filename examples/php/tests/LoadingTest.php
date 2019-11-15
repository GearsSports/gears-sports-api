<?php

require_once __DIR__ . '/../vendor/autoload.php';

use PHPUnit\Framework\TestCase;

use Gears\Proto\Capture\Capture;
use Gears\Proto\Capture\CaptureType;
use Gears\Proto\Capture\GraphFrameCollection;
use Gears\Proto\Player\Player;
use Gears\Proto\Player\Gender;
use Gears\Proto\Player\Handed;


class LoadingTest extends TestCase {

    private $testFolder = __DIR__."/../../data/";

    public function testLoadCapture()
    {
        // This example shows how to load a capture from a file
        $file = $this->testFolder . "Rickie Fowler/2013-10-07/31 Capture.gpcap";
        $this->assertTrue(file_exists($file));

        $capture = new Capture();
        $capture->mergeFromString(file_get_contents($file));

        $this->assertEquals('9c84191f-3712-4803-8a3b-432eb55a2bb1',$capture->getId());
        $this->assertEquals(CaptureType::GOLF,$capture->getType());
        $this->assertEquals(10,$capture->getFrames()->count());
    }

    public function testLoadPlayer()
    {
        // This example shows how to load a player from a file
        $file = $this->testFolder . "Rickie Fowler/PlayerInfo.bin";
        $this->assertTrue(file_exists($file));

        $player = new Player();
        $player->mergeFromString(file_get_contents($file));

        $this->assertEquals('777dcf9b-d40f-4260-8009-016ffce2650c',$player->getId());
        $this->assertEquals('',$player->getEmail());
        $this->assertEquals('Rickie',$player->getFirstName());
        $this->assertEquals('',$player->getMiddleName());
        $this->assertEquals('Fowler',$player->getLastName());
        $this->assertEquals(Gender::MALE,$player->getGender());
        $this->assertEquals(Handed::RIGHT_HANDED,$player->getHandedness());
        $this->assertNotNull($player->getCreatedAt());
        $this->assertNotNull($player->getUpdatedAt());
    }

    public function testLoadGraphData()
    {
        // This example shows how to load the graph data from a file
        $file = $this->testFolder . "Rickie Fowler/2013-10-07/48208_10.graphs";
        $this->assertTrue(file_exists($file));

        $graphFrameCollection = new GraphFrameCollection();
        $graphFrameCollection->mergeFromString(file_get_contents($file));

        $this->assertGreaterThanOrEqual(1,$graphFrameCollection->getFrames()->count());
    }
}
