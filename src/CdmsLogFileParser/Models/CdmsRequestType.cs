using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdmsLogFileParser.Models
{
    // LabelCheckMix CheckMixRequest
    // Check Job_Request
    // Check Job_Response
    // LabelCheckMix complete
    // AnswerQuestionRequest
    // Answer request
    // Answer Job_Response
    // AnswerQuestion complete

    public enum CdmsRequestType
    {
        Unknown,
        NA,
        ProductListRequest,
        ProductListResponse,
        LabelCheckMixCheckMixRequest,
        CheckJob_Request,
        CheckJob_Response,
        LabelCheckMixComplete,
        AnswerQuestionRequest,
        AnswerRequest,
        AnswerJob_Response,
        AnswerQuestionComplete
    }
}
