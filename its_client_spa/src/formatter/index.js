import moment from "moment";

export default {
  getAllResponse({meta, currentList}) {
    return {
      list: currentList,
      total: meta.totalElement
    }
  },

  getAllRequest({pagination, search, sortByStr}) {
    if (!sortByStr) {
      sortByStr = pagination.sortBy;
    }
    const descStr = pagination.descending ? 'desc' : 'asc';
    const orderBy = `${sortByStr}_${descStr}`;
    return {
      orderBy,
      pageIndex: pagination.page,
      pageSize: pagination.rowsPerPage,
      searchValue: search
    };
  },

  getPlanDayText(planDay,startDay){
    switch (planDay) {
      case "0":
      case 0:
        return "Chưa lên lịch";
      default:
        if (startDay) {
          return moment(startDay)
            .add(planDay - 1, "days")
            .format('DD/MM/YYYY');
        } else {
          return `Ngày ${planDay}`
        }
    }
  },

  getDaysObj(planDay,startDay){
    return {
      planDayText: this.getPlanDayText(planDay, startDay),
      planDay: _.toNumber(planDay),
      key: `day_${planDay}`,
    }
  },
}
