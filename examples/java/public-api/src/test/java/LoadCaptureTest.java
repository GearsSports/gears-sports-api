import com.gearssports.protobuf.capture.Capture;
import com.gearssports.protobuf.capture.CaptureType;
import org.junit.Test;

import java.io.IOException;
import java.util.UUID;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;

/**
 * Examples related to captures.
 */
public class LoadCaptureTest {

    /**
     * Shows how to load a capture.
     */
    @Test
    public void loadCapture() throws IOException {
        String captureResource = "data/Rickie Fowler/2013-10-07/31 Capture.gpcap";
        Capture capture = Capture.parseFrom(getClass().getClassLoader().getResourceAsStream(captureResource));

        assertNotNull(capture);
        UUID expectedId = UUID.fromString("9c84191f-3712-4803-8a3b-432eb55a2bb1");
        assertEquals(expectedId, UUID.fromString(capture.getId()));
        assertEquals(CaptureType.GOLF, capture.getType());
        assertEquals(10, capture.getFramesCount());
    }
}
