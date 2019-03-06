import com.gearssports.protobuf.player.Gender;
import com.gearssports.protobuf.player.Handed;
import com.gearssports.protobuf.player.Player;
import org.junit.Test;

import java.io.IOException;
import java.util.UUID;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;

/**
 * Examples related to players.
 */
public class LoadPlayerTest {

    /**
     * Shows how to load a player.
     */
    @Test
    public void loadPlayer() throws IOException {
        String playerResource = "data/Rickie Fowler/PlayerInfo.bin";
        Player player = Player.parseFrom(getClass().getClassLoader().getResourceAsStream(playerResource));

        assertNotNull(player);
        UUID expectedId = UUID.fromString("777dcf9b-d40f-4260-8009-016ffce2650c");
        assertEquals(expectedId, UUID.fromString(player.getId()));
        assertEquals("", player.getEmail());
        assertEquals("Rickie", player.getFirstName());
        assertEquals("", player.getMiddleName());
        assertEquals("Fowler", player.getLastName());
        assertEquals(Gender.MALE, player.getGender());
        assertEquals(Handed.RIGHT_HANDED, player.getHandedness());
        assertNotNull(player.getCreatedAt());
        assertNotNull(player.getUpdatedAt());
    }
}
