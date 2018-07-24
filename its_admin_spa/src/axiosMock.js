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
    })

    .onGet('/api/Question/categories')
    .reply((config) => {
      return [200, {
        currentList: [
          ['Nơi ở','Ăn uống','Ăn uống','Nơi ở','Ăn uống','Chi phí','Chi phí','Chi phí', ]
        ]
      }];
    })
    .onPost('api/Question')
    .reply((config) => {
      return [200, {
      }];
    })

    .onDelete('api/Question')
    .reply((config) => {
      return [200, {
      }];
    })

    .onGet('api/tag')
    .reply((config) => {
      return [200, {
        meta: {
          pageIndex: 1,
          pageSize: 5,
          totalElement: 4,
          totalPage: 1,
          orderBy: "name_asc",
          searchValue: null
        },
        currentList: [
          {
            id: 2,
            name: "Bình dân",
            categories: "Giá",
            locationCount: 2
          },
          {
            id: 3,
            name: "Chuyên món chay",
            categories: "Ăn chay",
            locationCount: 0
          },
          {
            id: 4,
            name: "Có món chay",
            categories: "Ăn chay",
            locationCount: 0
          },
          {
            id: 12,
            name: "gần trung tâm",
            categories: "Địa hình",
            locationCount: 0
          }
        ]
      }];
    })



    .onPost('api/Question')
    .reply((config) => {
      return [200, {
      }];
    })






}
