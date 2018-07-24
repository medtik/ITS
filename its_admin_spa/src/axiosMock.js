import MockAdapter from "axios-mock-adapter";

export default (axiosInstance) => {
  const mock = new MockAdapter(axiosInstance);

  mock
    .onGet('/api/Question')
    .reply((config) => {
      return [200, {
        meta: {
          pageIndex: 1,
          pageSize: 5,
          totalElement: 5,
          totalPage: 1,
          orderBy: 'content_asc',
          searchValue: null
        },
        currentList: [
          {
            id: '36',
            content: 'Bạn muốn chi tiêu như thế nào cho chuyến đi ?',
            categories: 'Nơi ở',
            answerCount: 2
          }
        ]
      }];
    });
}
