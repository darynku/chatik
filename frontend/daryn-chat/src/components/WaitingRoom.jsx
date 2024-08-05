import { Button, Card, Input, Typography } from "@material-tailwind/react";
import { useState } from "react";

export function WaitingRoom({ joinToChat }) {
  const [userName, setUserName] = useState();
  const [chatRoom, setChatRoom] = useState();

  const onSubmit = (a) => {
    a.preventDefault();
    joinToChat(userName, chatRoom);
  };
  return (
    <Card className="flex items-center justify-center min-h-screen">
      <Typography variant="h2" color="black" className="pb-5">Welcome To Chat</Typography>
      <form
        onSubmit={onSubmit}
        className="sm:w-96 bg-gray-100 p-16">
        <div className=" mb-1 flex flex-col gap-6">
          <Typography variant="h4" color="blue-gray" className="-mb-3">
            Name
          </Typography>
          <Input
            size="lg"
            className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
            placeholder="UserName"
            onChange={(e) => setUserName(e.target.value)}
          />
          <br />
          <Typography variant="h4" color="blue-gray" className="-mb-3">Chat Name</Typography>
          <Input
            size="lg"
            className=" !border-t-blue-gray-200 focus:!border-t-gray-900"
            placeholder="ChatRoom"
            onChange={(e) => setChatRoom(e.target.value)}
          />
          <br />
          <Button type="submit">submit</Button>
        </div>
      </form>
    </Card>
  );
}
