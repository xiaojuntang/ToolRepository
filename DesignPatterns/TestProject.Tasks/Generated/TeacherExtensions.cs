// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace UserServiceClient
{
    using System.Threading.Tasks;
   using Models;

    /// <summary>
    /// Extension methods for Teacher.
    /// </summary>
    public static partial class TeacherExtensions
    {
            /// <summary>
            /// 查询老师是否是结构化的用户
            /// 只要账号存在于学校管理后台，则认为是结构化的，否则为非结构化的
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='teacherId'>
            /// 教师Id
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            public static ResponseEntityBoolean GetSFlag(this ITeacher operations, int teacherId, string sign)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((ITeacher)s).GetSFlagAsync(teacherId, sign), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// 查询老师是否是结构化的用户
            /// 只要账号存在于学校管理后台，则认为是结构化的，否则为非结构化的
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='teacherId'>
            /// 教师Id
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<ResponseEntityBoolean> GetSFlagAsync(this ITeacher operations, int teacherId, string sign, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetSFlagWithHttpMessagesAsync(teacherId, sign, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// 根据老师Id获取老师信息
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='teacherId'>
            /// 老师Id
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            public static ResponseEntityTTeacher GetTeacherInfo(this ITeacher operations, int teacherId, string sign)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((ITeacher)s).GetTeacherInfoAsync(teacherId, sign), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// 根据老师Id获取老师信息
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='teacherId'>
            /// 老师Id
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<ResponseEntityTTeacher> GetTeacherInfoAsync(this ITeacher operations, int teacherId, string sign, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetTeacherInfoWithHttpMessagesAsync(teacherId, sign, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// 根据学校Id获取老师列表，带分页
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='pageIndex'>
            /// 页号
            /// </param>
            /// <param name='pageSize'>
            /// 页大小
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            public static PageResponseEntityListTTeacher GetPagedTeacherList(this ITeacher operations, int schoolId, int pageIndex, int pageSize, string sign)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((ITeacher)s).GetPagedTeacherListAsync(schoolId, pageIndex, pageSize, sign), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// 根据学校Id获取老师列表，带分页
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='pageIndex'>
            /// 页号
            /// </param>
            /// <param name='pageSize'>
            /// 页大小
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<PageResponseEntityListTTeacher> GetPagedTeacherListAsync(this ITeacher operations, int schoolId, int pageIndex, int pageSize, string sign, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetPagedTeacherListWithHttpMessagesAsync(schoolId, pageIndex, pageSize, sign, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// 根据学校Id和老师名称，模糊查询老师列表，带分页
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='keyWords'>
            /// 模糊教师名称
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='pageIndex'>
            /// 页号
            /// </param>
            /// <param name='pageSize'>
            /// 页大小
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            public static PageResponseEntityListTTeacher GetPagedTeacherListByKeyWords(this ITeacher operations, string keyWords, int schoolId, int pageIndex, int pageSize, string sign)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((ITeacher)s).GetPagedTeacherListByKeyWordsAsync(keyWords, schoolId, pageIndex, pageSize, sign), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// 根据学校Id和老师名称，模糊查询老师列表，带分页
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='keyWords'>
            /// 模糊教师名称
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='pageIndex'>
            /// 页号
            /// </param>
            /// <param name='pageSize'>
            /// 页大小
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<PageResponseEntityListTTeacher> GetPagedTeacherListByKeyWordsAsync(this ITeacher operations, string keyWords, int schoolId, int pageIndex, int pageSize, string sign, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetPagedTeacherListByKeyWordsWithHttpMessagesAsync(keyWords, schoolId, pageIndex, pageSize, sign, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// 根据学校ID查询本学校所有老师信息
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            public static ResponseEntityListTTeacher GetAllTeacherBySchoolId(this ITeacher operations, int schoolId, string sign)
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((ITeacher)s).GetAllTeacherBySchoolIdAsync(schoolId, sign), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// 根据学校ID查询本学校所有老师信息
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='schoolId'>
            /// 学校ID
            /// </param>
            /// <param name='sign'>
            /// 参数签名
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<ResponseEntityListTTeacher> GetAllTeacherBySchoolIdAsync(this ITeacher operations, int schoolId, string sign, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetAllTeacherBySchoolIdWithHttpMessagesAsync(schoolId, sign, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
