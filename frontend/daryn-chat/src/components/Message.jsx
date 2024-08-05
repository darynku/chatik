export const Message = ({ messageInfo }) => {
  return (
    <div className="mb-3">
      <h1 className="font-bold text-gray-700">{messageInfo.userName}</h1>
      <p className="text-gray-600">{messageInfo.message}</p>
    </div>
  );
};
