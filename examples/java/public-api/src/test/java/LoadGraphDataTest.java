import com.gearssports.protobuf.capture.GraphFrameCollection;
import org.junit.Test;

import java.io.IOException;
import java.util.UUID;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertNotNull;

/**
 * Examples related to graph frame collection.
 */
public class LoadGraphDataTest {

    /**
     * Shows how to load graph frames.
     */
    @Test
    public void loadCapture() throws IOException {
        String captureResource = "data/Rickie Fowler/2013-10-07/48208_10.graphs";
        GraphFrameCollection collection = GraphFrameCollection.parseFrom(getClass().getClassLoader().getResourceAsStream(captureResource));

        assertNotNull(collection);
        assertTrue(collection.getFramesCount() > 1);
    }
}
