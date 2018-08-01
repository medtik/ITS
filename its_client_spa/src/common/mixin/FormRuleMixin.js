const rules = {
  locationName: (value) => !!value || 'Tên địa điểm không được trống',
  questionContent: (value) => !!value || 'Vui lòng nhập nội dung câu hỏi',
  answerContent: (value) => !!value || 'Nội dung câu trả lời không được để trống',
  answerTag: (value) => !!value || 'Vui lòng gắn thẻ cho câu trả lời',
};

export default {
  data() {
    return {
      rules
    }
  }
}
