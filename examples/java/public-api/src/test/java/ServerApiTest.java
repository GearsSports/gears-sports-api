import com.gearssports.protobuf.api.v1.*;
import com.gearssports.protobuf.player.Player;
import com.gearssports.protobuf.server.CaptureInfo;
import com.google.protobuf.InvalidProtocolBufferException;
import com.google.protobuf.util.JsonFormat;
import io.grpc.ManagedChannel;
import io.grpc.Metadata;
import io.grpc.netty.shaded.io.grpc.netty.GrpcSslContexts;
import io.grpc.netty.shaded.io.grpc.netty.NettyChannelBuilder;
import io.grpc.netty.shaded.io.netty.handler.ssl.SslContext;
import io.grpc.stub.MetadataUtils;
import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.Test;

import javax.net.ssl.SSLException;
import java.io.InputStream;
import java.util.Objects;
import java.util.Scanner;
import java.util.UUID;
import java.util.concurrent.TimeUnit;

import static org.junit.Assert.*;

/**
 * Class containing tests that demonstrate how to access the gears sports public api.
 */
public class ServerApiTest {

    /**
     * Port used to connect to our servers.
     */
    private static final int port = 443;
    /**
     * The sandbox domain we have setup for testing.
     */
    private static final String domain;
    /**
     * Path to the resource that contains the certs needed to connect to our servers.
     */
    private static final String certsResource;
    /**
     * Path to the resource that contains a json representation of the gears.proto.api.v1.TokenRequest message.
     * <br/>
     * This resource should be modified by people running these examples so it contains the api credentials
     * provided by gears sports customer service.
     *
     * @see com.gearssports.protobuf.api.v1.TokenRequest This is the java class generated from that protobuf message.
     */
    private static final String tokenRequestResource;
    /**
     * Path to where you will need to add your own valid api credentials.
     * <br/>
     * <b>Warning:</b> The examples in this class will fail if valid credentials have not been provided.
     */
    private static final String tokenRequestFilePath;
    /**
     * The grpc channel we will use for all tests in this class.
     */
    private static ManagedChannel channel;
    /**
     * The response to our call to the gears authentication endpoint.
     * Since tokens can be used until they expire and the time it takes to
     * run all tests is shorter than the expiration time on the token we
     * will re-use it for all other calls made to gears endpoints.
     */
    private static TokenResponse token;

    static {
        domain = "devrpc.gearssports.com";
        certsResource = "data/public.pem";
        tokenRequestResource = "data/TokenRequest.json";
        tokenRequestFilePath = "<repo_root>/examples/java/public-api/src/test/resources/" + tokenRequestResource;
    }

    @BeforeClass
    public static void setUpBeforeClass() throws SSLException {
        InputStream certs = ServerApiTest.class.getClassLoader().getResourceAsStream(certsResource);
        SslContext sslContext = GrpcSslContexts.forClient().trustManager(certs).build();
        channel = NettyChannelBuilder.forAddress(domain, port)
                .sslContext(sslContext)
                .build();
    }

    @AfterClass
    public static void tearDownAfterClass() throws Exception {
        if (channel == null)
            return;
        channel.shutdown().awaitTermination(5, TimeUnit.SECONDS);
    }

    /**
     * @return Cached response from a call to the gears authentication endpoint.
     */
    private static TokenResponse getToken() throws InvalidProtocolBufferException {
        if (token == null) {
            InputStream stream = ServerApiTest.class.getClassLoader().getResourceAsStream(tokenRequestResource);
            //noinspection CharsetObjectCanBeUsed
            Scanner scanner = new Scanner(Objects.requireNonNull(stream), "UTF-8");
            String json = scanner.useDelimiter("\\A").next();
            TokenRequest.Builder builder = TokenRequest.newBuilder();
            JsonFormat.parser().merge(json, builder);
            TokenRequest request = builder.build();

            assertNotEquals(
                    "You have not updated the client id in " + tokenRequestFilePath,
                    "Add your client id here.",
                    request.getClientId()
            );
            assertNotEquals(
                    "You have not updated the client secret in " + tokenRequestFilePath,
                    "Add your client secret here.",
                    request.getClientSecret()
            );

            // Make grpc request for an auth token that we will need to make requests
            // to all other gears grpc methods.
            token = AuthServiceGrpc.newBlockingStub(channel).token(request);
        }
        return token;
    }

    /**
     * Builds the header that needs to be added to calls to gears endpoints.
     *
     * @return Auth header that needs to be added to calls to gears endpoints.
     */
    private static Metadata getAuthHeader() throws InvalidProtocolBufferException {
        Metadata header = new Metadata();
        Metadata.Key<String> key = Metadata.Key.of("authorization", Metadata.ASCII_STRING_MARSHALLER);
        header.put(key, "Bearer " + getToken().getAccessToken());
        return header;
    }

    /**
     * Example of authentication needed to access the server.
     */
    @Test
    public void authenticate() throws InvalidProtocolBufferException {
        TokenResponse response = getToken();
        assertNotNull(response);
        assertNotEquals("", response.getAccessToken());
        assertTrue(response.getExpiresIn() > 0);
    }

    /**
     * Example of getting a list of players from the server.
     */
    @Test
    public void listPlayers() throws InvalidProtocolBufferException {
        PlayerServiceGrpc.PlayerServiceBlockingStub stub = PlayerServiceGrpc.newBlockingStub(channel);

        // Attach auth headers.
        stub = MetadataUtils.attachHeaders(stub, getAuthHeader());

        // Setup request data.
        ListPlayersRequest request = ListPlayersRequest.newBuilder()
                .setPerPage(2)
                // see com.gearssports.protobuf.api.v1.ListPlayersFilters for filtering options.
                .build();

        //Make request for a list of players.
        ListPlayersResponse response = stub.list(request);

        assertNotNull(response);
        assertTrue(response.getPage() > 0);
        assertEquals(request.getPerPage(), response.getPerPage());
        assertTrue(response.getTotalCount() >= response.getPlayersCount());
        for (Player player : response.getPlayersList()) {
            assertNotEquals("", player.getId());
            UUID playerId = UUID.fromString(player.getId());
            assertEquals(player.getId(), playerId.toString());
        }
    }

    /**
     * Example of getting a list of captures from the server.
     */
    @Test
    public void listCaptures() throws InvalidProtocolBufferException {
        CaptureServiceGrpc.CaptureServiceBlockingStub stub = CaptureServiceGrpc.newBlockingStub(channel);

        // Attach auth headers.
        stub = MetadataUtils.attachHeaders(stub, getAuthHeader());

        // Setup request data.
        ListCapturesRequest request = ListCapturesRequest.newBuilder()
                .setPerPage(2)
                // see com.gearssports.protobuf.api.v1.ListCapturesFilters for filtering options.
                .build();

        //Make request for a list of captures.
        ListCapturesResponse response = stub.list(request);

        assertNotNull(response);
        assertTrue(response.getPage() > 0);
        assertEquals(request.getPerPage(), response.getPerPage());
        assertTrue(response.getTotalCount() >= response.getCapturesCount());
        for (CaptureInfo captureInfo : response.getCapturesList()) {
            assertNotEquals("", captureInfo.getId());
            UUID captureId = UUID.fromString(captureInfo.getId());
            assertEquals(captureInfo.getId(), captureId.toString());

            // A CaptureInfo instance only contains basic info about a capture.
            // If you want the full capture object you can download it using the url returned by captureInfo.getUrl().
            // The url returned by captureInfo.getUrl() should not be stored as it will expire a few minutes after it is generated.
            assertNotNull(captureInfo.getUrl());
        }
    }
}
