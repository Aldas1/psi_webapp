import { useQuery } from "@tanstack/react-query";
import { getUsersLeaderboard } from "../api/leaderboard";
import {
  Heading,
  Table,
  TableContainer,
  Tbody,
  Td,
  Th,
  Thead,
  Tr,
  VStack,
} from "@chakra-ui/react";

function UserLeaderboard() {
  const userLeaderboardQuery = useQuery({
    queryKey: ["userLeaderboard"],
    queryFn: getUsersLeaderboard,
  });

  if (userLeaderboardQuery.isLoading) return "Loading...";
  if (userLeaderboardQuery.error || userLeaderboardQuery.data === undefined)
    return "An error has occurred";

  const leaderboard = userLeaderboardQuery.data;

  return (
    <VStack>
      <Heading>User leaderboard</Heading>
      <TableContainer>
        <Table>
          <Thead>
            <Tr>
              <Th>Username</Th>
              <Th>Average score</Th>
              <Th>Number of submissions</Th>
            </Tr>
          </Thead>
          <Tbody>
            {leaderboard.map((item) => {
              return (
                <Tr key={item.username}>
                  <Td>{item.username}</Td>
                  <Td>{item.averageScore.toFixed(2)}</Td>
                  <Td>{item.numberOfSubmissions}</Td>
                </Tr>
              );
            })}
          </Tbody>
        </Table>
      </TableContainer>
    </VStack>
  );
}

export default UserLeaderboard;
